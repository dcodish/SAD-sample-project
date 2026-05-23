#!/usr/bin/env bash
# Regenerate the distributable Word + PowerPoint docs from their markdown sources.
# Run from the repo root after editing any of the source .md files.
#
# Requires: pandoc on PATH (or edit PANDOC below), python on PATH.
set -e

PANDOC="${PANDOC:-pandoc}"

# PREREQS + PROMPTS_CHEATSHEET: title page (BGU logo), per-chapter page breaks, TOC.
# Generated with the styled reference.docx (Heading1 = pageBreakBefore, Centered style),
# then fix_toc.py moves the TOC below the title page and inserts a page break so the
# title page is a full page.
"$PANDOC" PREREQS.md            -o PREREQS.docx            --reference-doc=reference.docx --toc --standalone
"$PANDOC" PROMPTS_CHEATSHEET.md -o PROMPTS_CHEATSHEET.docx --reference-doc=reference.docx --toc --standalone
python fix_toc.py PREREQS.docx PROMPTS_CHEATSHEET.docx

# Slide deck: PowerPoint (one slide per `#`) + a Word version.
"$PANDOC" SLIDE_DECK.md -o SLIDE_DECK.pptx --slide-level=1
"$PANDOC" SLIDE_DECK.md -o SLIDE_DECK.docx --toc --standalone

echo "Docs regenerated. Open the .docx files in Word once so the TOC field populates, then save."
