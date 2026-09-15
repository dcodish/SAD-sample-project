#!/usr/bin/env python
"""Check the group's analysis PDFs before Phase 2 extraction, and pull their
text out once so Claude doesn't have to read the PDFs page-by-page as images.

Why this exists: a PDF that has a real text layer is cheap to read. A PDF that
is only pictures of pages (a scan, a phone photo, or "export as image") has no
text layer, so every page has to be looked at as an image. That is what turns a
20-minute extraction step into an hour and a half, and it burns the token quota
on the way.

Usage (uv installs pymupdf on the fly - nothing to install first):

    uv run --with pymupdf cloned/scripts/check_pdfs.py docs

    # render specific pages as PNG - use for the diagram pages only
    uv run --with pymupdf cloned/scripts/check_pdfs.py docs/PartB.pdf --render 12,15

Writes one .txt per PDF into docs/_extracted/ (override with --out). Those are
work files for Phase 2; they can be deleted afterwards.
"""

import argparse
import sys
from pathlib import Path

# The Windows console is rarely UTF-8, and this script prints Hebrew file names.
try:
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
except Exception:
    pass

# A page needs at least this many non-space characters to count as a text page.
# Real prose pages run in the thousands; a scanned page extracts 0, and a page
# that is one big diagram with a caption extracts a few dozen.
TEXT_PAGE_MIN_CHARS = 100


def collect_pdfs(paths):
    pdfs = []
    for raw in paths:
        p = Path(raw)
        if p.is_dir():
            pdfs += sorted(q for q in p.rglob("*.pdf") if "_extracted" not in q.parts)
        elif p.suffix.lower() == ".pdf":
            pdfs.append(p)
        else:
            print(f"  skipping (not a PDF): {p}")
    return pdfs


def inspect(pdf, out_dir, write_text):
    import fitz  # PyMuPDF

    doc = fitz.open(pdf)
    per_page = []
    chunks = []
    for i, page in enumerate(doc, start=1):
        text = page.get_text()
        per_page.append(len("".join(text.split())))
        chunks.append(f"\n\n===== page {i} =====\n\n{text}")

    total_pages = len(per_page)
    text_pages = [i for i, n in enumerate(per_page, 1) if n >= TEXT_PAGE_MIN_CHARS]
    thin_pages = [i for i, n in enumerate(per_page, 1) if n < TEXT_PAGE_MIN_CHARS]
    doc.close()

    txt_path = None
    if write_text and text_pages:
        out_dir.mkdir(parents=True, exist_ok=True)
        txt_path = out_dir / (pdf.stem + ".txt")
        txt_path.write_text("".join(chunks), encoding="utf-8")

    share = len(text_pages) / total_pages if total_pages else 0
    if not text_pages:
        verdict, advice = "NO TEXT LAYER", (
            "Every page is a picture. Re-export this PDF from the original "
            "Word/Visual Paradigm file (File > Save as PDF), or hand Claude the "
            "original .docx instead. Do not scan or photograph it."
        )
    elif share < 0.5:
        verdict, advice = "MOSTLY IMAGES", (
            "Most pages have no text layer. Re-export from the original file if "
            "you still have it; otherwise expect this step to be slow."
        )
    else:
        verdict, advice = "OK", (
            "Text layer present. Read the .txt file instead of the PDF, and only "
            "open the PDF itself for the diagram pages listed below."
        )

    return {
        "pdf": pdf, "pages": total_pages, "chars": sum(per_page),
        "text_pages": len(text_pages), "thin_pages": thin_pages,
        "size_mb": pdf.stat().st_size / (1024 * 1024),
        "verdict": verdict, "advice": advice, "txt": txt_path,
    }


def render(pdf, pages, out_dir, dpi):
    import fitz

    doc = fitz.open(pdf)
    out_dir.mkdir(parents=True, exist_ok=True)
    for n in pages:
        if not 1 <= n <= len(doc):
            print(f"  page {n} is out of range (1-{len(doc)})")
            continue
        dest = out_dir / f"{pdf.stem}-p{n}.png"
        doc[n - 1].get_pixmap(dpi=dpi).save(dest)
        print(f"  rendered page {n} -> {dest}")
    doc.close()


def main():
    ap = argparse.ArgumentParser(description=__doc__,
                                 formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("paths", nargs="+", help="PDF files, or folders to scan for PDFs")
    ap.add_argument("--out", default=None,
                    help="where to write .txt/.png (default: <first path>/_extracted)")
    ap.add_argument("--render", default=None,
                    help="comma-separated page numbers to render as PNG, e.g. 12,15")
    ap.add_argument("--dpi", type=int, default=200, help="render resolution (default 200)")
    ap.add_argument("--no-text", action="store_true", help="report only, write no .txt files")
    args = ap.parse_args()

    first = Path(args.paths[0])
    out_dir = Path(args.out) if args.out else (first if first.is_dir() else first.parent) / "_extracted"

    pdfs = collect_pdfs(args.paths)
    if not pdfs:
        print("No PDFs found. Put your Part A / Part B PDFs in docs/ first.")
        return 1

    if args.render:
        pages = [int(n) for n in args.render.replace(" ", "").split(",") if n]
        for pdf in pdfs:
            print(f"\n{pdf}")
            render(pdf, pages, out_dir, args.dpi)
        return 0

    results = [inspect(p, out_dir, not args.no_text) for p in pdfs]

    print(f"\n{'file':<34} {'pages':>5} {'with text':>10} {'MB':>6}  verdict")
    print("-" * 78)
    for r in results:
        print(f"{r['pdf'].name[:34]:<34} {r['pages']:>5} "
              f"{r['text_pages']:>10} {r['size_mb']:>6.1f}  {r['verdict']}")

    for r in results:
        print(f"\n{r['pdf'].name}")
        print(f"  {r['advice']}")
        if r["txt"]:
            print(f"  text written to: {r['txt']}  ({r['chars']:,} characters)")
        if r["txt"]:
            print("  Sanity-check it: open that .txt and read the first few lines. "
                  "Hebrew words should be intact.")
            print("  (Numbers, parentheses and English inside a Hebrew line often land in "
                  "an odd spot - that is normal for extracted RTL text, not a broken file. "
                  "The PDF stays the source of truth wherever a line is ambiguous.)")
        if r["thin_pages"] and r["verdict"] != "NO TEXT LAYER":
            shown = ",".join(str(n) for n in r["thin_pages"][:20])
            more = " ..." if len(r["thin_pages"]) > 20 else ""
            print(f"  pages with little or no text (likely full-page diagrams): {shown}{more}")

    worst = [r for r in results if r["verdict"] != "OK"]
    print("\n" + "=" * 78)
    if worst:
        print("ACTION NEEDED before extracting: " +
              ", ".join(r["pdf"].name for r in worst))
        print("Re-export those from the original file, then run this check again.")
    else:
        print("All PDFs have a usable text layer. Extract from the .txt files in "
              f"{out_dir}, and open the PDF only for the diagram pages listed above.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
