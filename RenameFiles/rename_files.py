import os
import shutil

def batch_rename(input_folder, output_folder, prefix='', suffix='', start_number=1):
    # 确保输出文件夹存在
    if not os.path.exists(output_folder):
        os.makedirs(output_folder)
    
    # 获取输入文件夹中的所有文件
    files = os.listdir(input_folder)
    
    # 对文件进行排序（可选）
    files.sort()
    
    # 遍历所有文件并重命名
    for index, filename in enumerate(files, start=start_number):
        # 获取文件扩展名
        file_ext = os.path.splitext(filename)[1]
        
        # 创建新的文件名
        new_filename = f"{prefix}{index:03d}{suffix}{file_ext}"
        
        # 构建完整的文件路径
        old_path = os.path.join(input_folder, filename)
        new_path = os.path.join(output_folder, new_filename)
        
        # 复制并重命名文件
        shutil.copy2(old_path, new_path)
        print(f"重命名: {filename} -> {new_filename}")

# 使用示例
input_folder = r"C:\Users\Awith\Desktop\test"
output_folder = r"C:\Users\Awith\Desktop\testout"
batch_rename(input_folder, output_folder, prefix="_", suffix="_", start_number=1)
