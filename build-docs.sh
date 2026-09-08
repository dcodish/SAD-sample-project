#!/usr/bin/env bash
# Regenerate the distributable student handouts from their markdown sources.
# Run from the repo root after editing any of the source .md files.
#
# Output: self-contained single-file HTML in dist/ — no external assets, no
# fonts to download, works offline, opens in any browser, and prints to PDF
# cleanly (Ctrl+P -> Save as PDF) if a paper copy is wanted.
#
# Requires: pandoc on PATH (or set PANDOC=/path/to/pandoc).
set -e

PANDOC="${PANDOC:-pandoc}"
OUT="dist"
TEMPLATE="html/template.html"
SLIDES="html/slides.html"

mkdir -p "$OUT"

# Documents: source markdown -> handout HTML.
DOCS=(
  "PREREQS.md"
  "PROMPTS_CHEATSHEET.md"
  "LESSON_STEPS.md"
  "GIT_GROUP_WORKFLOW.md"
)

for src in "${DOCS[@]}"; do
  dest="$OUT/${src%.md}.html"
  echo "  $src -> $dest"
  "$PANDOC" "$src" \
    -f markdown+yaml_metadata_block \
    -t html5 \
    --standalone \
    --embed-resources \
    --template="$TEMPLATE" \
    --toc --toc-depth=2 \
    --syntax-highlighting=tango \
    -V logo=bgu-logo.png \
    -o "$dest"
done

# Slide deck: a real self-contained HTML presentation (one slide per `#`).
# Navigate with arrow keys / space / a presenter clicker; O = overview grid,
# F = fullscreen; Ctrl+P prints one slide per landscape page.
echo "  SLIDE_DECK.md -> $OUT/SLIDE_DECK.html"
"$PANDOC" SLIDE_DECK.md   -f markdown+yaml_metadata_block   -t html5   --standalone   --embed-resources   --section-divs   --template="$SLIDES"   -V logo=bgu-logo.png   -o "$OUT/SLIDE_DECK.html"

# A PowerPoint fallback is no longer built - the deck is HTML. If you ever need
# .pptx for a room that demands it, uncomment the next line (note: pandoc does
# not set RTL paragraph direction in PowerPoint, so Hebrew comes out left-aligned).
# "$PANDOC" SLIDE_DECK.md -o "$OUT/SLIDE_DECK.pptx" --slide-level=1

# Cross-document links are written as .md in the sources (so they work on
# GitHub), but the handouts are .html. Rewrite them so the links resolve in
# the published set too.
python - "$OUT" <<'PYFIX'
import glob, io, os, re, sys
out = sys.argv[1]
docs = ["PREREQS", "PROMPTS_CHEATSHEET", "LESSON_STEPS", "GIT_GROUP_WORKFLOW", "MCP_SETUP", "SLIDE_DECK"]
HEB = re.compile(r"[֐-׿]")
TAG = re.compile(r"<[^>]+>")

def classify(html):
    """Tag each blockquote as a pasteable English prompt or a Hebrew note.
    Done here rather than in JS so the page is correct before scripts run,
    and still correct if they never do."""
    out, i, n = [], 0, 0
    for m in re.finditer(r"<blockquote>", html):
        end = html.find("</blockquote>", m.end())
        inner = html[m.end():end if end != -1 else m.end() + 400]
        text = TAG.sub("", inner)[:200]
        cls = "note" if HEB.search(text) else "prompt"
        out.append(html[i:m.start()]); out.append('<blockquote class="%s">' % cls)
        i = m.end(); n += 1
    out.append(html[i:])
    return "".join(out), n
pat = re.compile(r'href="(\./)?(' + "|".join(docs) + r')\.md"')
for f in glob.glob(os.path.join(out, "*.html")):
    s = io.open(f, encoding="utf-8").read()
    n, k = pat.subn(lambda m: 'href="%s.html"' % m.group(2), s)
    n, c = classify(n)
    # Guard: a placeholder like <entity> written outside backticks is parsed as
    # an HTML tag and renders as nothing, silently eating it from the prompt.
    KNOWN = set("""a p br em strong code pre ul ol li div span h1 h2 h3 h4 h5 h6 blockquote
    table thead tbody tr th td hr img sub sup nav main header section html head body meta
    title style script link button del ins figure figcaption dl dt dd col colgroup label
    input""".split())
    body = n[n.find("<main"):]
    stray = sorted({t for t in re.findall(r"<([A-Za-z_][A-Za-z0-9_.\-]*)[^>]*>", body)
                    if t.lower() not in KNOWN})
    if stray:
        print("  !! WARNING %s: placeholders parsed as HTML and will render blank: %s"
              % (os.path.basename(f), ", ".join("<%s>" % t for t in stray)))
        print("     Wrap them in backticks in the markdown source.")
    if k or c:
        io.open(f, "w", encoding="utf-8", newline="").write(n)
        print("  fixups: %s (links=%d, blockquotes=%d)" % (os.path.basename(f), k, c))
PYFIX

echo
echo "Done. Handouts are in $OUT/ - hand out the .html files directly."
