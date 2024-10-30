import tkinter as tk
from tkinter import filedialog, messagebox
import markdown2
import os
import webbrowser
from bs4 import BeautifulSoup
import tempfile

def convert_md_to_html(md_file):
    with open(md_file, 'r', encoding='utf-8') as f:
        md_content = f.read()
    
    html_content = markdown2.markdown(md_content)
    soup = BeautifulSoup(html_content, 'html.parser')
    
    # 为HTML元素添加类
    for i in range(1, 7):
        for heading in soup.find_all(f'h{i}'):
            heading['class'] = heading.get('class', []) + ['heading']
    
    for p in soup.find_all('p'):
        p['class'] = p.get('class', []) + ['paragraph']
    
    for a in soup.find_all('a'):
        a['class'] = a.get('class', []) + ['link']
    
    for ul in soup.find_all('ul'):
        ul['class'] = ul.get('class', []) + ['list']
    for ol in soup.find_all('ol'):
        ol['class'] = ol.get('class', []) + ['list']
    
    for pre in soup.find_all('pre'):
        pre['class'] = pre.get('class', []) + ['code-block']
        code = pre.find('code')
        if code:
            code['class'] = code.get('class', []) + ['code']
    
    for code in soup.find_all('code'):
        if code.parent.name != 'pre':
            code['class'] = code.get('class', []) + ['inline-code']
    
    return soup.prettify()

def preview_html():
    md_file = md_entry.get()
    css_file = css_entry.get()
    
    if not md_file or not css_file:
        messagebox.showerror("错误", "请选择 Markdown 文件和 CSS 文件")
        return
    
    html_content = convert_md_to_html(md_file)
    
    with open(css_file, 'r', encoding='utf-8') as f:
        css_content = f.read()
    
    preview_html = f"""
    <!DOCTYPE html>
    <html lang="zh-CN">
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>预览</title>
        <style>
        {css_content}
        </style>
    </head>
    <body>
        <div class="container">
            {html_content}
        </div>
    </body>
    </html>
    """
    
    # 创建临时文件
    with tempfile.NamedTemporaryFile(delete=False, suffix='.html', mode='w', encoding='utf-8') as temp_file:
        temp_file.write(preview_html)
        temp_file_path = temp_file.name
    
    # 在默认浏览器中打开临时文件
    webbrowser.open('file://' + os.path.realpath(temp_file_path))

def convert():
    md_file = md_entry.get()
    css_file = css_entry.get()
    output_file = output_entry.get()
    
    if not md_file or not css_file or not output_file:
        messagebox.showerror("错误", "请选择所有必要的文件")
        return
    
    try:
        html_content = convert_md_to_html(md_file)
        
        with open(css_file, 'r', encoding='utf-8') as f:
            css_content = f.read()
        
        full_html = f"""
        <!DOCTYPE html>
        <html lang="zh-CN">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>转换结果</title>
            <style>
            {css_content}
            </style>
        </head>
        <body>
            <div class="container">
                {html_content}
            </div>
        </body>
        </html>
        """
        
        with open(output_file, 'w', encoding='utf-8') as f:
            f.write(full_html)
        
        messagebox.showinfo("成功", f"转换完成！输出文件：{output_file}")
    except Exception as e:
        messagebox.showerror("错误", f"转换过程中出现错误：{str(e)}")

# 创建主窗口
root = tk.Tk()
root.title("Markdown转HTML工具")

# 配置行和列的权重，使其能够自适应调整大小
root.grid_columnconfigure(1, weight=1)
for i in range(5):  # 减少一行，因为移除了上传按钮
    root.grid_rowconfigure(i, weight=1)

# Markdown文件选择
tk.Label(root, text="选择Markdown文件：").grid(row=0, column=0, sticky="e", padx=5, pady=5)
md_entry = tk.Entry(root, width=50)
md_entry.grid(row=0, column=1, sticky="ew", padx=5, pady=5)
tk.Button(root, text="浏览", command=lambda: md_entry.insert(0, filedialog.askopenfilename(filetypes=[("Markdown Files", "*.md")]))).grid(row=0, column=2, padx=5, pady=5)

# CSS文件选择
tk.Label(root, text="选择CSS文件：").grid(row=1, column=0, sticky="e", padx=5, pady=5)
css_entry = tk.Entry(root, width=50)
css_entry.grid(row=1, column=1, sticky="ew", padx=5, pady=5)
tk.Button(root, text="浏览", command=lambda: css_entry.insert(0, filedialog.askopenfilename(filetypes=[("CSS Files", "*.css")]))).grid(row=1, column=2, padx=5, pady=5)

# 输出文件选择
tk.Label(root, text="选择输出HTML文件：").grid(row=2, column=0, sticky="e", padx=5, pady=5)
output_entry = tk.Entry(root, width=50)
output_entry.grid(row=2, column=1, sticky="ew", padx=5, pady=5)
tk.Button(root, text="浏览", command=lambda: output_entry.insert(0, filedialog.asksaveasfilename(defaultextension=".html", filetypes=[("HTML Files", "*.html")]))).grid(row=2, column=2, padx=5, pady=5)

# 预览按钮
tk.Button(root, text="预览", command=preview_html, width=20).grid(row=3, column=1, pady=10)

# 转换按钮
tk.Button(root, text="转换", command=convert, width=20).grid(row=4, column=1, pady=10)

root.mainloop()
