package validation

import (
	"errors"
	"fmt"
	"strings"

	"github.com/go-playground/validator/v10"
)

var validate = validator.New()

func ValidateStruct(s interface{}) string {
	err := validate.Struct(s)
	if err == nil {
		return ""
	}

	var errorMessages []string
	var vErrs validator.ValidationErrors
	if errors.As(err, &vErrs) {
		for _, e := range vErrs {
			msg := fmt.Sprintf("%s(%s)", e.Field(), e.ActualTag())
			if e.Param() != "" {
				msg += "=" + e.Param()
			}
			errorMessages = append(errorMessages, msg)
		}
	}

	return strings.Join(errorMessages, ", ")
}
