package main

import (
	"log"
	"os"
	"report-entrance/internal/api"
	"report-entrance/internal/utils/mysql"

	"github.com/joho/godotenv"
)

func init() {
	err := godotenv.Load(".env")
	if err != nil {
		log.Fatalf("Error loading .env file. %v\n", err)
	}
}

func main() {
	mysql.Connect()
	// auth.InitUser()
	r := api.SetupRouter()
	// Listen and Server in 0.0.0.0:8080
	r.Run(":" + os.Getenv("SERVER_PORT"))
}
