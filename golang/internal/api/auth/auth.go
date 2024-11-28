package auth

import (
	"html"
	"net/http"
	"report-entrance/internal/dao/user"
	"report-entrance/internal/model"
	"report-entrance/internal/utils/action"
	"report-entrance/internal/utils/token"
	"strings"

	"github.com/gin-gonic/gin"
	"golang.org/x/crypto/bcrypt"
)

// api/login 的请求体
type UserBody struct {
	Username string `json:"username" binding:"required"`
	Password string `json:"password" binding:"required"`
}

func Login(c *gin.Context) {
	var req UserBody
	if err := c.ShouldBindBodyWithJSON(&req); err != nil {
		action.Bad(c, http.StatusBadRequest, err.Error())
		return
	}

	u := model.User{
		Username: req.Username,
		Password: req.Password,
	}

	// 调用 models.LoginCheck 对用户名和密码进行验证
	token, err := loginCheck(u.Username, u.Password)
	if err != nil {
		action.Bad(c, http.StatusBadRequest, "用户名或密码错误！")
		return
	}
	action.Ok(c, "登录成功！", gin.H{
		"token": token,
	})
}

func CurrentUser(c *gin.Context) {
	// 从token中解析出user_id
	user_id, err := token.ExtractTokenID(c)
	if err != nil {
		action.Bad(c, http.StatusBadRequest, err.Error())
		return
	}

	// 根据user_id从数据库查询数据
	u, err := user.GetUserByID(user_id)
	if err != nil {
		action.Bad(c, http.StatusBadRequest, err.Error())
		return
	}
	action.Ok(c, "用户获取成功！", gin.H{
		"user": u,
	})
}

func Register(c *gin.Context) {
	var req UserBody

	if err := c.ShouldBindBodyWithJSON(&req); err != nil {
		action.Bad(c, http.StatusBadRequest, "请求参数错误！")
		return
	}

	u := model.User{
		Username: req.Username,
		Password: req.Password,
	}

	if err := beforeSave(&u); err != nil {
		action.Bad(c, http.StatusInternalServerError, "添加用户失败！")
		return
	}
	if err := user.Insert(u); err != nil {
		action.Bad(c, http.StatusInternalServerError, "添加用户失败！")
	}
	action.Ok(c, "添加用户成功！", "")
}

func verifyPassword(password, hashedPassword string) error {
	return bcrypt.CompareHashAndPassword([]byte(hashedPassword), []byte(password))
}

func loginCheck(username, password string) (string, error) {

	u, err := user.GetUserByName(username)

	if err != nil {
		return "", err
	}
	err = verifyPassword(password, u.Password)
	if err != nil && err == bcrypt.ErrMismatchedHashAndPassword {
		return "", err
	}

	token, err := token.GenerateToken(u.ID)
	if err != nil {
		return "", err
	}
	return token, nil
}

func beforeSave(u *model.User) error {
	hashedPassword, err := bcrypt.GenerateFromPassword([]byte(u.Password), bcrypt.DefaultCost)
	if err != nil {
		return err
	}
	u.Password = string(hashedPassword)
	u.Username = html.EscapeString(strings.TrimSpace(u.Username))
	return nil
}

func InitUser() {

	u := model.User{
		Username: "admin",
		Password: "123456",
	}

	if err := beforeSave(&u); err != nil {
		// action.Bad(c, http.StatusInternalServerError, "添加用户失败！")
		return
	}
	if err := user.Insert(u); err != nil {
		// action.Bad(c, http.StatusInternalServerError, "添加用户失败！")
		return
	}
}
