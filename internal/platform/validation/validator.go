package validation

import (
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

	for _, err := range err.(validator.ValidationErrors) {
		var msg string

		switch err.Tag() {
		case "required":
			msg = fmt.Sprintf("%s alanı zorunludur, boş bırakılamaz", err.Field())
		case "url":
			msg = fmt.Sprintf("%s alanı geçerli bir internet adresi (URL) olmalıdır", err.Field())
		case "oneof":
			msg = fmt.Sprintf("%s alanı sadece şu değerleri alabilir: %s", err.Field(), err.Param())
		case "email":
			msg = fmt.Sprintf("%s alanı geçerli bir e-posta adresi olmalıdır", err.Field())
		case "min":
			msg = fmt.Sprintf("%s alanı en az %s karakter/değer olmalıdır", err.Field(), err.Param())
		case "max":
			msg = fmt.Sprintf("%s alanı en fazla %s karakter/değer olabilir", err.Field(), err.Param())
		case "len":
			msg = fmt.Sprintf("%s alanı tam olarak %s karakter uzunluğunda olmalıdır", err.Field(), err.Param())
		case "numeric":
			msg = fmt.Sprintf("%s alanı sadece rakamlardan oluşmalıdır", err.Field())
		case "alphanum":
			msg = fmt.Sprintf("%s alanı sadece harf ve rakam içermelidir", err.Field())
		case "gt":
			msg = fmt.Sprintf("%s alanı gelecekteki bir tarih olmalıdır", err.Field())
		case "gte":
			msg = fmt.Sprintf("%s alanı %s değerinden büyük veya eşit olmalıdır", err.Field(), err.Param())
		case "lte":
			msg = fmt.Sprintf("%s alanı %s değerinden küçük veya eşit olmalıdır", err.Field(), err.Param())
		case "boolean":
			msg = fmt.Sprintf("%s alanı true veya false olmalıdır", err.Field())
		case "datetime":
			msg = fmt.Sprintf("%s alanı belirtilen tarih formatına uymalıdır: %s", err.Field(), err.Param())
		default:
			msg = fmt.Sprintf("%s alanı doğrulama hatası aldı: %s", err.Field(), err.Tag())
		}

		errorMessages = append(errorMessages, msg)
	}

	return strings.Join(errorMessages, "\n")
}
