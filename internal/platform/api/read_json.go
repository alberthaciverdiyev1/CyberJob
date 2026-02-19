package api

import (
	"encoding/json"
	"errors"
	"fmt"
	"io"
	"net/http"
	"strings"
)

const maxBytes = 1_048_576

// ReadJSON - Request body'yi güvenli şekilde okur ve dst'ye decode eder.
// Aşağıdaki hataları yakalar ve anlamlı mesajlar döner:
//   - Bilinmeyen field
//   - Yanlış tip (örn. string yerine int)
//   - Eksik ya da bozuk JSON
//   - Body boyut aşımı
//   - Birden fazla JSON objesi
func ReadJSON(w http.ResponseWriter, r *http.Request, dst any) error {
	r.Body = http.MaxBytesReader(w, r.Body, maxBytes)

	dec := json.NewDecoder(r.Body)

	dec.DisallowUnknownFields()

	if err := dec.Decode(dst); err != nil {
		return translateDecodeError(err)
	}

	if err := dec.Decode(&struct{}{}); !errors.Is(err, io.EOF) {
		return errors.New("request body must contain only one JSON object")
	}

	return nil
}

func translateDecodeError(err error) error {
	var (
		syntaxErr        *json.SyntaxError
		unmarshalTypeErr *json.UnmarshalTypeError
		maxBytesErr      *http.MaxBytesError
	)

	switch {
	case errors.As(err, &syntaxErr):
		return fmt.Errorf("request body contains malformed JSON (at position %d)", syntaxErr.Offset)

	case errors.Is(err, io.ErrUnexpectedEOF):
		return errors.New("request body contains malformed JSON")

	case errors.As(err, &unmarshalTypeErr):
		if unmarshalTypeErr.Field != "" {
			return fmt.Errorf("invalid type for field %q: expected %s", unmarshalTypeErr.Field, unmarshalTypeErr.Type)
		}
		return fmt.Errorf("invalid type at position %d: expected %s", unmarshalTypeErr.Offset, unmarshalTypeErr.Type)

	case strings.HasPrefix(err.Error(), "json: unknown field "):
		fieldName := strings.TrimPrefix(err.Error(), "json: unknown field ")
		return fmt.Errorf("unknown field %s", fieldName)

	case errors.Is(err, io.EOF):
		return errors.New("request body must not be empty")

	case errors.As(err, &maxBytesErr):
		return fmt.Errorf("request body must not exceed %d bytes", maxBytesErr.Limit)

	default:
		return err
	}
}
