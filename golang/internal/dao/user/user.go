package user

import (
	"report-entrance/internal/model"
	"report-entrance/internal/utils/mysql"
)

func GetUserByName(username string) (model.User, error) {
	u := model.User{}

	result := mysql.Conn.Model(model.User{}).Where("username = ?", username).Take(&u)
	if result.Error != nil {
		return u, result.Error
	}
	return u, nil
}
func Insert(user model.User) error {
	result := mysql.Conn.Create(&user)
	if result.Error != nil {
		return result.Error
	}
	return nil
}

func GetUserByID(uid uint) (model.User, error) {
	var u model.User
	if err := mysql.Conn.First(&u, uid).Error; err != nil {
		return u, err
	}

	u.Password = ""
	return u, nil
}
