package model

type DeclareStatus int

const (
	Booked DeclareStatus = 0
	Processing
	Completed
)

type Declare struct {
	BaseModel
	Subject     string         `json:"subject" binding:"required"`
	License     string         `json:"license" binding:"required"`
	LegalPerson string         `json:"legalPerson" binding:"required"`
	LegalNumber string         `gorm:"unique" json:"legalNumber" binding:"required"`
	LegalIdCard string         `json:"legalIdCard" binding:"required"`
	Status      *DeclareStatus `json:"status" binding:"required"`
	Desc        string         `json:"desc"`
}
