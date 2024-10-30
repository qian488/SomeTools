# rename_files_gui.py - 高级批量文件重命名工具

## 程序介绍
这是一个带有图形用户界面的高级批量文件重命名工具。它允许用户选择输入和输出文件夹，设置文件名前缀、后缀和起始编号，并支持基于文件属性的复杂重命名规则。该工具特别适用于需要对大量文件进行规范化命名的场景，如整理照片、文档或其他类型的文件集合。

## 主要功能
1. 选择输入和输出文件夹：用户可以浏览并选择包含要重命名文件的文件夹，以及存放重命名后文件的目标文件夹。
2. 设置文件名前缀和后缀：允许用户为新文件名添加自定义的前缀和后缀。
3. 设置起始编号：用户可以指定文件编号的起始值，默认为1。
4. 批量重命名文件：程序会自动为所有文件重命名，保持原始文件扩展名不变。
5. 显示重命名进度：通过进度条实时显示重命名过程的完成情况。
6. 支持创建新文件夹：用户可以直接在界面中创建新的输出文件夹。
7. 文件类型过滤：允许用户指定要处理的文件类型。
8. 预览功能：在执行批量重命名前，用户可以预览更改效果。
9. 复杂重命名规则：支持基于文件属性（创建日期、修改日期、文件大小）的重命名。

## 使用方法
1. 环境准备：
   - 确保已安装 Python（推荐 Python 3.6 或更高版本）
   - 确保已安装 tkinter 库（通常随 Python 标准库一起安装）
2. 运行程序：
   - 打开命令行或终端
   - 导航到程序所在目录
   - 执行命令：`python rename_files_gui.py`
3. 在打开的图形界面中：
   - 点击"浏览"按钮选择输入和输出文件夹，或使用"新建文件夹"按钮创建新的输出文件夹
   - 在相应的输入框中设置前缀、后缀（可选）
   - 设置起始编号（默认为1，可根据需要修改）
   - 指定要处理的文件类型（用逗号分隔，如 "jpg,png,gif"）
   - 选择命名规则（默认、创建日期、修改日期、文件大小）
   - 点击"预览"按钮查看重命名效果
   - 点击"开始重命名"按钮启动批量重命名过程
4. 等待进度条完成，程序会显示处理完成的消息

## 界面说明
- 输入文件夹：选择包含需要重命名的文件的源文件夹
- 输出文件夹：选择或创建用于存储重命名后文件的目标文件夹
- 前缀：将添加到每个新文件名开头的文本（可选）
- 后缀：将添加到每个新文件名编号之后、扩展名之前的文本（可选）
- 起始编号：文件编号的起始值，程序将从这个数字开始为文件编号
- 文件类型：指定要处理的文件扩展名（用逗号分隔）
- 命名规则：选择重命名规则（默认、创建日期、修改日期、文件大小）
- 预览按钮：打开预览窗口，显示重命名效果

## 代码结构和解释

### 导入必要的库
```python
import os
import shutil
import tkinter as tk
from tkinter import filedialog, messagebox, ttk, simpledialog
import threading
import queue
from concurrent.futures import ThreadPoolExecutor
import datetime
```

新增的 datetime 库用于处理文件的创建和修改时间。

### 新增函数

#### 获取文件信息
```python
def get_file_info(file_path):
    stat = os.stat(file_path)
    return {
        'creation_time': datetime.datetime.fromtimestamp(stat.st_ctime),
        'modification_time': datetime.datetime.fromtimestamp(stat.st_mtime),
        'size': stat.st_size
    }
```

此函数获取文件的创建时间、修改时间和大小，用于复杂的命名规则。

#### 生成新文件名
```python
def generate_new_filename(filename, index, prefix, suffix, naming_rule):
    file_ext = os.path.splitext(filename)[1]
    file_info = get_file_info(filename)
    
    if naming_rule == "默认":
        new_name = f"{prefix}{index:03d}{suffix}{file_ext}"
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
```

此函数根据选择的命名规则生成新文件名。

#### 预览重命名
```python
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
```

此函数创建预览窗口，显示原文件名和新文件名。

### 更新的函数

#### 批量重命名线程函数
```python
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
```

此函数现在包含了文件类型过滤和新的命名规则支持。

#### 主要重命名函数
```python
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
```

此函数现在包含了新的用户输入（文件类型和命名规则）。

### GUI 创建
程序的GUI部分新增了以下元素：
- 文件类型输入框：用于指定要处理的文件扩展名
- 命名规则下拉菜单：用于选择重命名规则
- 预览按钮：用于打开预览窗口

## 注意事项
- 程序会创建文件副本，原文件保持不变，这样可以防止意外丢失数据
- 确保有足够的磁盘空间存储重命名后的文件，特别是当处理大量或大型文件时
- 大量文件处理可能需要一些时间，请耐心等待。进度条会给出完成情况的指示
- 程序使用多线程处理，可能会占用较多系统资源，建议在处理大量文件时关闭其他不必要的应用程序
- 预览功能仅显示前100个文件，以避免处理大量文件时的性能问题
- 新的命名规则可能会导致文件名较长，请确保文件系统支持长文件名
- 文件类型过滤区分大小写，请使用小写扩展名

## 技术特点
- 使用多线程处理提高效率，特别适合处理大量文件
- 实时显示处理进度，提供良好的用户体验
- 支持大量文件的批量处理，适用于各种文件整理场景
- 使用 ThreadPoolExecutor 实现并行处理，充分利用多核 CPU
- 通过队列（Queue）实现线程间的安全通信，确保 GUI 更新的稳定性
- 支持基于文件属性的复杂重命名规则
- 提供文件类型过滤功能，增加处理的灵活性
- 包含预览功能，让用户可以在执行操作前查看效果

## 可能的未来改进
- 实现撤销功能，允许用户在操作后恢复原始文件名
- 添加更多的命名规则选项，如EXIF数据（对于图片文件）
- 实现拖放功能，使文件夹选择更加便捷
- 添加配置保存和加载功能，方便用户保存常用设置
- 优化大量文件的预览性能，可能通过分页或虚拟滚动实现
- 添加日志功能，记录重命名操作的详细信息
- 实现批处理模式，允许用户保存重命名任务并稍后执行
