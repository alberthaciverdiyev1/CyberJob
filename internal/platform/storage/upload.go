package storage

import (
	"fmt"
	"io"
	"os"
	"path/filepath"
	"time"
)

func UploadFile(file io.Reader, ext string, folder string) (string, error) {
	fileName := fmt.Sprintf("%d%s", time.Now().UnixNano(), ext)

	uploadDir := filepath.Join("public", "uploads", folder)

	if err := os.MkdirAll(uploadDir, os.ModePerm); err != nil {
		return "", fmt.Errorf("could not create directory: %w", err)
	}
	fullPath := filepath.Join(uploadDir, fileName)

	dst, err := os.Create(fullPath)
	if err != nil {
		return "", fmt.Errorf("could not create file: %w", err)
	}
	defer dst.Close()

	if _, err := io.Copy(dst, file); err != nil {
		return "", fmt.Errorf("could not copy file content: %w", err)
	}

	return fullPath, nil
}
