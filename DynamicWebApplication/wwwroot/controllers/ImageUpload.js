class CustomUploadAdapter {
    constructor(loader) {
        this.loader = loader;
    }

    upload() {
        return this.loader.file
            .then(file => new Promise((resolve, reject) => {
                const data = new FormData();
                data.append('upload', file);

                fetch('/upload/images', {
                    method: 'POST',
                    body: data,
                })
                    .then(response => response.json())
                    .then(result => {
                        if (result.uploaded) {
                            resolve({
                                default: result.url // Adjust depending on the API response
                            });
                        } else {
                            reject(result.error.message);
                        }
                    })
                    .catch(error => {
                        reject('Upload failed');
                    });
            }));
    }

    abort() {
        // Handle aborting the upload process
    }
}
userAvatar.onchange = evt => {
    const [file] = ImageFile.files
    if (file) {
        preview.src = URL.createObjectURL(file);
    }
}