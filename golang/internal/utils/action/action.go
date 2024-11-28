package action

import (
	"net/http"

	"github.com/gin-gonic/gin"
)

func Bad(c *gin.Context, status int, message string) {
	c.JSON(status, gin.H{"status": "bad", "message": message})
}

func Ok(c *gin.Context, message string, data interface{}) {
	c.JSON(http.StatusOK, gin.H{"status": "ok", "data": data, "message": message})
}

func Result(c *gin.Context, status int, message string, data interface{}) {
	c.JSON(status, gin.H{"status": "ok", "data": data, "message": message})
}
