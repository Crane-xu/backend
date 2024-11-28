package mysql

import (
	"fmt"
	"log"
	"os"
	"report-entrance/internal/model"

	"gorm.io/driver/mysql"
	"gorm.io/gorm"
)

var (
	Conn *gorm.DB
)

func Connect() {

	DbHost := os.Getenv("DB_HOST")
	DbPort := os.Getenv("DB_PORT")
	DbUser := os.Getenv("DB_USER")
	DbPass := os.Getenv("DB_PASS")
	DbName := os.Getenv("DB_NAME")

	// 参考 https://github.com/go-sql-driver/mysql#dsn-data-source-name 获取详情
	dsn := fmt.Sprintf("%s:%s@tcp(%s:%s)/%s?charset=utf8mb4&parseTime=True&loc=Local", DbUser, DbPass, DbHost, DbPort, DbName)
	db, err := gorm.Open(mysql.Open(dsn), &gorm.Config{})
	if err != nil {
		log.Panic("open db_mysql error ", err)
	}
	db.AutoMigrate(&model.User{}, &model.Declare{})
	Conn = db
}
