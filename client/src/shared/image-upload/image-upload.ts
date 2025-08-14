import { Component, input, output, signal } from '@angular/core';

@Component({
  selector: 'app-image-upload',
  imports: [],
  templateUrl: './image-upload.html',
  styleUrl: './image-upload.css'
})
export class ImageUpload {
  protected imageSrc = signal<string | ArrayBuffer | null | undefined>(null);
  protected isDragging = false;
  private filetoUpload: File | null = null;
  uploadFile = output<File>();
  loading = input<boolean>(false);

  onDragOver(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = true;
  }
    onDragLeave(event: DragEvent) {
      event.preventDefault();
      event.stopPropagation();
      this.isDragging = true;
    }
    onDrop(event: DragEvent) {
      event.preventDefault();
      event.stopPropagation();
      this.isDragging = false;

      if(event.dataTransfer?.files.length){
        const file = event.dataTransfer.files[0];
        this.previewImage(file);
        this.filetoUpload = file;
      }
    }

    onCancel() {
      this.imageSrc.set(null);
      this.filetoUpload = null;
    }

    onUploadFile() {
      if (this.filetoUpload) {
        this.uploadFile.emit(this.filetoUpload);
      }
    }

    private previewImage(file: File) {
      const reader = new FileReader();
      reader.onload = (e) => {
        this.imageSrc.set(e.target?.result);
      };
      reader.readAsDataURL(file);
    }
}
