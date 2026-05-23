import sys, re, zipfile, shutil, os

def fix(path):
    # read document.xml from the docx
    with zipfile.ZipFile(path) as z:
        names = z.namelist()
        data = {n: z.read(n) for n in names}
    xml = data['word/document.xml'].decode('utf-8')

    # 1. extract the single TOC sdt block
    m = re.search(r'<w:sdt>.*?</w:sdt>', xml, re.S)
    if not m:
        print(f'{path}: no TOC sdt found, skipping'); return
    toc = m.group(0)
    xml = xml[:m.start()] + xml[m.end():]

    # 2. find the Author paragraph (last title-page line) and insert after it:
    #    a page break (so the title page is a full page) + the TOC block
    pb = '<w:p><w:r><w:br w:type="page"/></w:r></w:p>'
    am = re.search(r'<w:p\b[^>]*>(?:(?!</w:p>).)*?w:val="Author".*?</w:p>', xml, re.S)
    if not am:
        print(f'{path}: no Author paragraph found, skipping'); return
    insert_at = am.end()
    xml = xml[:insert_at] + pb + toc + xml[insert_at:]

    data['word/document.xml'] = xml.encode('utf-8')

    # 3. rewrite the docx
    tmp = path + '.tmp'
    with zipfile.ZipFile(tmp, 'w', zipfile.ZIP_DEFLATED) as z:
        for n in names:
            z.writestr(n, data[n])
    os.replace(tmp, path)
    print(f'{path}: TOC moved below title page + page break inserted')

for p in sys.argv[1:]:
    fix(p)
