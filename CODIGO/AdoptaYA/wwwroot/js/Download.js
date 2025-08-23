window.downloadFromByteArray = function (filename, byteBase64, mimeType) {
    const link = document.createElement('a');
    link.href = "data:" + mimeType + ";base64," + byteBase64;
    link.download = filename;
    link.click();
}
