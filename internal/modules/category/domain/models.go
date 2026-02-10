package domain

type CreateCategoryRequest struct {
	Name     string `json:"name" validate:"required,min=3,max=100"`
	Icon     string `json:"icon" validate:"required"`
	ParentID uint   `json:"parent_id" validate:"omitempty,numeric"`
}

type CategoryResponse struct {
	ID       uint               `json:"id"`
	Name     string             `json:"name"`
	Icon     string             `json:"icon"`
	ParentID uint               `json:"parent_id,omitempty"`
	Children []CategoryResponse `json:"children,omitempty"`
}

type UpdateCategoryRequest struct {
	ID       uint   `json:"id" validate:"required"`
	Name     string `json:"name" validate:"omitempty,min=3"`
	Icon     string `json:"icon" validate:"omitempty"`
	ParentID uint   `json:"parent_id" validate:"omitempty,numeric"`
}
