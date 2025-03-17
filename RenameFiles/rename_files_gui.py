import os
import shutil
import tkinter as tk
from tkinter import filedialog, messagebox, ttk, simpledialog
import threading
import queue
from concurrent.futures import ThreadPoolExecutor
import datetime

def select_folder(entry):
    folder = filedialog.askdirectory()
    if folder:
        entry.delete(0, tk.END)
        entry.insert(0, folder)

def create_new_folder(entry):
    parent_dir = filedialog.askdirectory(title="选择新文件夹的父目录")
    if parent_dir:
        new_folder_name = simpledialog.askstring("新文件夹", "输入新文件夹名称：")
        if new_folder_name:
            new_folder_path = os.path.join(parent_dir, new_folder_name)
            try:
                os.makedirs(new_folder_path)
                entry.delete(0, tk.END)
                entry.insert(0, new_folder_path)
                messagebox.showinfo("成功", f"已创建新文件夹：{new_folder_path}")
            except OSError as e:
                messagebox.showerror("错误", f"创建文件夹失败：{e}")

def rename_file(args):
    old_path, new_path = args
    shutil.copy2(old_path, new_path)
    return 1

def get_file_info(file_path):
    stat = os.stat(file_path)
    return {
        'creation_time': datetime.datetime.fromtimestamp(stat.st_ctime),
        'modification_time': datetime.datetime.fromtimestamp(stat.st_mtime),
        'size': stat.st_size
    }

def generate_new_filename(filename, index, prefix, suffix, naming_rule):
    file_ext = os.path.splitext(filename)[1]
    file_info = get_file_info(filename)
    
    if naming_rule == "默认":
        new_name = f"{prefix}{index:d}{suffix}{file_ext}"
    elif naming_rule == "创建日期":
        date_str = file_info['creation_time'].strftime("%Y%m%d")
        new_name = f"{prefix}{date_str}_{index:03d}{suffix}{file_ext}"
    elif naming_rule == "修改日期":
        date_str = file_info['modification_time'].strftime("%Y%m%d")
        new_name = f"{prefix}{date_str}_{index:03d}{suffix}{file_ext}"
    elif naming_rule == "文件大小":
        size_kb = file_info['size'] // 1024
        new_name = f"{prefix}{size_kb}KB_{index:03d}{suffix}{file_ext}"
    else:
        new_name = f"{prefix}{index:03d}{suffix}{file_ext}"
    
    return new_name

def batch_rename_thread(input_folder, output_folder, prefix, suffix, start_number, progress_queue, file_types, naming_rule):
    files = [f for f in os.listdir(input_folder) if os.path.splitext(f)[1].lower() in file_types]
    files.sort()
    total_files = len(files)

    rename_tasks = []
    for index, filename in enumerate(files, start=start_number):
        old_path = os.path.join(input_folder, filename)
        new_filename = generate_new_filename(old_path, index, prefix, suffix, naming_rule)
        new_path = os.path.join(output_folder, new_filename)
        rename_tasks.append((old_path, new_path))

    completed_files = 0
    with ThreadPoolExecutor(max_workers=os.cpu_count()) as executor:
        for result in executor.map(rename_file, rename_tasks):
            completed_files += result
            progress_queue.put((completed_files / total_files * 100, completed_files))

    progress_queue.put(None)  # 表示处理完成

def update_progress(progress_var, progress_bar, progress_queue, completed_files_label):
    try:
        progress = progress_queue.get_nowait()
        if progress is None:
            messagebox.showinfo("成功", f"文件重命名完成，共处理 {completed_files_label['text']} 个文件")
            progress_bar.grid_remove()
            completed_files_label.grid_remove()
        else:
            progress_percentage, completed_files = progress
            progress_var.set(progress_percentage)
            completed_files_label['text'] = completed_files
            root.after(10, update_progress, progress_var, progress_bar, progress_queue, completed_files_label)
    except queue.Empty:
        root.after(10, update_progress, progress_var, progress_bar, progress_queue, completed_files_label)

def preview_rename():
    input_folder = input_entry.get()
    prefix = prefix_entry.get()
    suffix = suffix_entry.get()
    start_number = int(start_number_entry.get())
    file_types = [f".{ext.strip()}" for ext in file_type_entry.get().split(",")]
    naming_rule = naming_rule_var.get()

    if not input_folder:
        messagebox.showerror("错误", "请选择输入文件夹")
        return

    files = [f for f in os.listdir(input_folder) if os.path.splitext(f)[1].lower() in file_types]
    files.sort()

    preview_window = tk.Toplevel(root)
    preview_window.title("重命名预览")
    preview_tree = ttk.Treeview(preview_window, columns=("原文件名", "新文件名"), show="headings")
    preview_tree.heading("原文件名", text="原文件名")
    preview_tree.heading("新文件名", text="新文件名")
    preview_tree.pack(fill=tk.BOTH, expand=1)

    for index, filename in enumerate(files[:100], start=start_number):  # 限制预览前100个文件
        old_path = os.path.join(input_folder, filename)
        new_filename = generate_new_filename(old_path, index, prefix, suffix, naming_rule)
        preview_tree.insert("", "end", values=(filename, new_filename))

def batch_rename():
    input_folder = input_entry.get()
    output_folder = output_entry.get()
    prefix = prefix_entry.get()
    suffix = suffix_entry.get()
    start_number = int(start_number_entry.get())
    file_types = [f".{ext.strip()}" for ext in file_type_entry.get().split(",")]
    naming_rule = naming_rule_var.get()

    if not input_folder or not output_folder:
        messagebox.showerror("错误", "请选择输入和输出文件夹")
        return

    if not os.path.exists(output_folder):
        os.makedirs(output_folder)

    progress_var = tk.DoubleVar()
    progress_bar = ttk.Progressbar(root, variable=progress_var, maximum=100)
    progress_bar.grid(row=9, column=0, columnspan=4, sticky="ew", padx=5, pady=5)

    completed_files_label = tk.Label(root, text="0")
    completed_files_label.grid(row=10, column=0, columnspan=4, sticky="ew", padx=5, pady=5)

    progress_queue = queue.Queue()
    threading.Thread(target=batch_rename_thread, args=(input_folder, output_folder, prefix, suffix, start_number, progress_queue, file_types, naming_rule), daemon=True).start()
    update_progress(progress_var, progress_bar, progress_queue, completed_files_label)

# 创建主窗口
root = tk.Tk()
root.title("批量重命名文件")

# 配置根窗口的网格
root.grid_columnconfigure(1, weight=1)
for i in range(11):  # 假设我们有11行
    root.grid_rowconfigure(i, weight=1)

# 输入文件夹
tk.Label(root, text="输入文件夹:").grid(row=0, column=0, sticky="e", padx=5, pady=5)
input_entry = tk.Entry(root)
input_entry.grid(row=0, column=1, sticky="ew", padx=5, pady=5)
tk.Button(root, text="浏览", command=lambda: select_folder(input_entry)).grid(row=0, column=2, padx=5, pady=5)
tk.Button(root, text="新建文件夹", command=lambda: create_new_folder(input_entry)).grid(row=0, column=3, padx=5, pady=5)

# 输出文件夹
tk.Label(root, text="输出文件夹:").grid(row=1, column=0, sticky="e", padx=5, pady=5)
output_entry = tk.Entry(root)
output_entry.grid(row=1, column=1, sticky="ew", padx=5, pady=5)
tk.Button(root, text="浏览", command=lambda: select_folder(output_entry)).grid(row=1, column=2, padx=5, pady=5)
tk.Button(root, text="新建文件夹", command=lambda: create_new_folder(output_entry)).grid(row=1, column=3, padx=5, pady=5)

# 前缀
tk.Label(root, text="前缀:").grid(row=2, column=0, sticky="e", padx=5, pady=5)
prefix_entry = tk.Entry(root)
prefix_entry.grid(row=2, column=1, sticky="ew", padx=5, pady=5)

# 后缀
tk.Label(root, text="后缀:").grid(row=3, column=0, sticky="e", padx=5, pady=5)
suffix_entry = tk.Entry(root)
suffix_entry.grid(row=3, column=1, sticky="ew", padx=5, pady=5)

# 起始编号
tk.Label(root, text="起始编号:").grid(row=4, column=0, sticky="e", padx=5, pady=5)
start_number_entry = tk.Entry(root)
start_number_entry.insert(0, "1")
start_number_entry.grid(row=4, column=1, sticky="ew", padx=5, pady=5)

# 文件类型过滤
tk.Label(root, text="文件类型 (用逗号分隔):").grid(row=5, column=0, sticky="e", padx=5, pady=5)
file_type_entry = tk.Entry(root)
file_type_entry.insert(0, "jpg,png,gif")
file_type_entry.grid(row=5, column=1, sticky="ew", padx=5, pady=5)

# 命名规则
tk.Label(root, text="命名规则:").grid(row=6, column=0, sticky="e", padx=5, pady=5)
naming_rule_var = tk.StringVar(value="默认")
naming_rule_options = ["默认", "创建日期", "修改日期", "文件大小"]
naming_rule_menu = ttk.Combobox(root, textvariable=naming_rule_var, values=naming_rule_options)
naming_rule_menu.grid(row=6, column=1, sticky="ew", padx=5, pady=5)

# 预览按钮
preview_button = tk.Button(root, text="预览", command=preview_rename)
preview_button.grid(row=7, column=1, pady=10, sticky="ew")

# 重命名按钮
rename_button = tk.Button(root, text="开始重命名", command=batch_rename)
rename_button.grid(row=8, column=1, pady=10, sticky="ew")

# 进度条和完成文件数标签（初始时隐藏）
progress_var = tk.DoubleVar()
progress_bar = ttk.Progressbar(root, variable=progress_var, maximum=100)
progress_bar.grid(row=9, column=0, columnspan=4, sticky="ew", padx=5, pady=5)
progress_bar.grid_remove()

completed_files_label = tk.Label(root, text="0")
completed_files_label.grid(row=10, column=0, columnspan=4, sticky="ew", padx=5, pady=5)
completed_files_label.grid_remove()

# 设置最小窗口大小
root.update()
root.minsize(root.winfo_width(), root.winfo_height())

root.mainloop()
