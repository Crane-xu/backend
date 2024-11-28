package declare

import (
	"report-entrance/internal/model"
	"report-entrance/internal/utils/mysql"

	"gorm.io/gorm"
)

func List(LegalNumber string) ([]model.Declare, error) {
	var declares []model.Declare
	var result *gorm.DB
	if LegalNumber == "" {
		result = mysql.Conn.Find(&declares)
	} else {
		result = mysql.Conn.Where("legal_number = ?", LegalNumber).Find(&declares)
	}

	if result.Error != nil {
		return []model.Declare{}, result.Error
	}
	return declares, nil
}

func Insert(declare model.Declare) error {
	result := mysql.Conn.Create(&declare)
	if result.Error != nil {
		return result.Error
	}
	return nil
}

func Update(declare model.Declare) error {
	result := mysql.Conn.Save(&declare)
	if result.Error != nil {
		return result.Error
	}
	return nil
}

func Delete(declareId uint) error {
	result := mysql.Conn.Delete(&model.Declare{}, declareId)
	if result.Error != nil {
		return result.Error
	}
	return nil
}

func GetDeclareByID(uid uint) (model.Declare, error) {
	var u model.Declare
	if err := mysql.Conn.First(&u, uid).Error; err != nil {
		return u, err
	}
	return u, nil
}

func SetStatus(uid uint, status model.DeclareStatus) error {
	result := mysql.Conn.Model(&model.Declare{}).Where("id = ?", uid).Update("status", status)
	if result.Error != nil {
		return result.Error
	}
	return nil
}
