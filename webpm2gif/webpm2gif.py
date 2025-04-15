import os
import tkinter as tk
from tkinter import filedialog, messagebox, ttk
from PIL import Image
import cv2
import numpy as np
import threading
import time
from datetime import datetime

def webp_to_gif(input_path, output_path):
    try:
        with Image.open(input_path) as img:
            img = img.resize((240, 240), Image.LANCZOS)
            img.save(output_path, 'GIF', optimize=True)
        return True
    except Exception as e:
        print(f"WebP 转换失败：{e}")
        return False

def webm_to_gif(input_path, output_path):
    try:
        cap = cv2.VideoCapture(input_path)
        if not cap.isOpened():
            raise Exception("无法打开视频文件")

        fps = int(cap.get(cv2.CAP_PROP_FPS))
        total_frames = int(cap.get(cv2.CAP_PROP_FRAME_COUNT))
        
        frames = []
        while True:
            ret, frame = cap.read()
            if not ret:
                break
                
            frame = cv2.resize(frame, (240, 240))
            frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            frames.append(Image.fromarray(frame))

        cap.release()

        if frames:
            frames[0].save(
                output_path,
                save_all=True,
                append_images=frames[1:],
                duration=int(1000/fps),
                loop=0,
                optimize=True
            )
            return True
        return False
    except Exception as e:
        print(f"WebM 转换失败：{e}")
        return False

class FileList(tk.Frame):
    def __init__(self, master, **kwargs):
        super().__init__(master, **kwargs)
        
        # 创建列表框和滚动条
        self.scrollbar = ttk.Scrollbar(self)
        self.listbox = tk.Listbox(self, yscrollcommand=self.scrollbar.set, selectmode=tk.EXTENDED)
        self.scrollbar.config(command=self.listbox.yview)
        
        # 创建按钮框架
        self.button_frame = tk.Frame(self)
        
        # 创建按钮
        self.remove_button = ttk.Button(self.button_frame, text="删除选中", command=self.remove_selected)
        self.clear_button = ttk.Button(self.button_frame, text="清空列表", command=self.clear_all)
        
        # 布局
        self.listbox.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)
        self.scrollbar.pack(side=tk.RIGHT, fill=tk.Y)
        
        self.remove_button.pack(side=tk.LEFT, padx=5)
        self.clear_button.pack(side=tk.LEFT, padx=5)
        self.button_frame.pack(fill=tk.X, pady=5)
        
    def add_files(self, files):
        for file in files:
            if file not in self.listbox.get(0, tk.END):
                self.listbox.insert(tk.END, file)
    
    def remove_selected(self):
        selected = self.listbox.curselection()
        for index in reversed(selected):
            self.listbox.delete(index)
    
    def clear_all(self):
        self.listbox.delete(0, tk.END)
    
    def get_files(self):
        return list(self.listbox.get(0, tk.END))

class App:
    def __init__(self, root):
        self.root = root
        self.root.title("微信表情包批量转换工具")
        self.root.geometry("600x500")
        
        # 创建主框架
        self.main_frame = ttk.Frame(root, padding="10")
        self.main_frame.pack(fill=tk.BOTH, expand=True)
        
        # 创建文件选择部分
        self.create_file_selection()
        
        # 创建文件列表
        self.create_file_list()
        
        # 创建输出目录选择
        self.create_output_selection()
        
        # 创建进度显示
        self.create_progress_display()
        
        # 创建转换按钮
        self.create_convert_button()
        
    def create_file_selection(self):
        frame = ttk.LabelFrame(self.main_frame, text="文件选择", padding="5")
        frame.pack(fill=tk.X, pady=5)
        
        # 创建按钮框架
        button_frame = ttk.Frame(frame)
        button_frame.pack(fill=tk.X, pady=5)
        
        # 创建按钮
        ttk.Button(button_frame, text="选择单个文件", command=self.select_single_file).pack(side=tk.LEFT, padx=5)
        ttk.Button(button_frame, text="选择多个文件", command=self.select_multiple_files).pack(side=tk.LEFT, padx=5)
        ttk.Button(button_frame, text="选择文件夹", command=self.select_directory).pack(side=tk.LEFT, padx=5)
    
    def create_file_list(self):
        frame = ttk.LabelFrame(self.main_frame, text="待转换文件", padding="5")
        frame.pack(fill=tk.BOTH, expand=True, pady=5)
        
        self.file_list = FileList(frame)
        self.file_list.pack(fill=tk.BOTH, expand=True)
    
    def create_output_selection(self):
        frame = ttk.LabelFrame(self.main_frame, text="输出设置", padding="5")
        frame.pack(fill=tk.X, pady=5)
        
        # 输出目录选择
        dir_frame = ttk.Frame(frame)
        dir_frame.pack(fill=tk.X, pady=5)
        
        ttk.Label(dir_frame, text="输出目录:").pack(side=tk.LEFT)
        self.output_dir_entry = ttk.Entry(dir_frame)
        self.output_dir_entry.pack(side=tk.LEFT, fill=tk.X, expand=True, padx=5)
        ttk.Button(dir_frame, text="浏览", command=self.select_output_dir).pack(side=tk.LEFT)
        
        # 输出命名选项
        name_frame = ttk.Frame(frame)
        name_frame.pack(fill=tk.X, pady=5)
        
        ttk.Label(name_frame, text="输出命名:").pack(side=tk.LEFT)
        self.name_var = tk.StringVar(value="timestamp")
        ttk.Radiobutton(name_frame, text="时间戳", variable=self.name_var, value="timestamp").pack(side=tk.LEFT, padx=5)
        ttk.Radiobutton(name_frame, text="序号", variable=self.name_var, value="number").pack(side=tk.LEFT, padx=5)
        ttk.Radiobutton(name_frame, text="原文件名", variable=self.name_var, value="original").pack(side=tk.LEFT, padx=5)
    
    def create_progress_display(self):
        frame = ttk.LabelFrame(self.main_frame, text="转换进度", padding="5")
        frame.pack(fill=tk.X, pady=5)
        
        self.progress_bar = ttk.Progressbar(frame, length=400, mode='determinate')
        self.progress_bar.pack(fill=tk.X, pady=5)
        
        self.status_label = ttk.Label(frame, text="准备就绪")
        self.status_label.pack(fill=tk.X)
    
    def create_convert_button(self):
        ttk.Button(self.main_frame, text="开始转换", command=self.convert_files).pack(pady=10)
    
    def select_single_file(self):
        file_path = filedialog.askopenfilename(
            title="选择文件",
            filetypes=[("WebP/WebM 文件", "*.webp *.webm")]
        )
        if file_path:
            self.file_list.add_files([file_path])
    
    def select_multiple_files(self):
        file_paths = filedialog.askopenfilenames(
            title="选择文件",
            filetypes=[("WebP/WebM 文件", "*.webp *.webm")]
        )
        if file_paths:
            self.file_list.add_files(file_paths)
    
    def select_directory(self):
        dir_path = filedialog.askdirectory(title="选择文件夹")
        if dir_path:
            files = []
            for root, _, filenames in os.walk(dir_path):
                for filename in filenames:
                    if filename.lower().endswith(('.webp', '.webm')):
                        files.append(os.path.join(root, filename))
            if files:
                self.file_list.add_files(files)
            else:
                messagebox.showinfo("提示", "所选文件夹中没有找到 WebP 或 WebM 文件")
    
    def select_output_dir(self):
        dir_path = filedialog.askdirectory()
        if dir_path:
            self.output_dir_entry.delete(0, tk.END)
            self.output_dir_entry.insert(0, dir_path)
    
    def get_output_filename(self, input_path, index):
        name, ext = os.path.splitext(os.path.basename(input_path))
        naming = self.name_var.get()
        
        if naming == "timestamp":
            timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
            return f"{name}_{timestamp}.gif"
        elif naming == "number":
            return f"{name}_{index:03d}.gif"
        else:  # original
            return f"{name}.gif"
    
    def convert_files(self):
        file_paths = self.file_list.get_files()
        output_dir = self.output_dir_entry.get()
        
        if not file_paths:
            messagebox.showerror("错误", "请选择要转换的文件")
            return
            
        if not output_dir:
            messagebox.showerror("错误", "请选择输出目录")
            return
        
        # 设置进度条
        self.progress_bar["maximum"] = len(file_paths)
        self.progress_bar["value"] = 0
        self.status_label.config(text="准备转换...")
        
        def convert_thread():
            success_count = 0
            fail_count = 0
            
            for i, input_path in enumerate(file_paths):
                if not input_path.strip():
                    continue
                    
                filename = os.path.basename(input_path)
                output_path = os.path.join(output_dir, self.get_output_filename(input_path, i))
                
                self.status_label.config(text=f"正在转换: {filename}")
                
                if input_path.lower().endswith('.webp'):
                    success = webp_to_gif(input_path, output_path)
                elif input_path.lower().endswith('.webm'):
                    success = webm_to_gif(input_path, output_path)
                else:
                    success = False
                    
                if success:
                    success_count += 1
                else:
                    fail_count += 1
                    
                self.progress_bar["value"] = i + 1
                self.root.update()
                
            self.status_label.config(text=f"转换完成！成功: {success_count}, 失败: {fail_count}")
            messagebox.showinfo("完成", f"转换完成！\n成功: {success_count} 个\n失败: {fail_count} 个")

        thread = threading.Thread(target=convert_thread)
        thread.start()

# 创建主窗口
root = tk.Tk()
app = App(root)
root.mainloop()