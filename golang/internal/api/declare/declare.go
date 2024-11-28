package declare

import (
	"net/http"
	"report-entrance/internal/dao/declare"
	"report-entrance/internal/model"
	"report-entrance/internal/utils/action"
	"strconv"

	"github.com/gin-gonic/gin"
)

// api/login 的请求体
type ReqSearch struct {
	Phone string `json:"phone" binding:"required"`
}

func SearchByPhone(c *gin.Context) {
	phone := c.Query("phone")

	list, err := declare.List(phone)
	if err != nil {
		action.Bad(c, http.StatusInternalServerError, "数据获取失败！")
		return
	}
	action.Ok(c, "列表获取成功！", list)
}

func GetList(c *gin.Context) {
	list, err := declare.List("")
	if err != nil {
		action.Bad(c, http.StatusInternalServerError, "数据获取失败！")
		return
	}
	action.Ok(c, "列表获取成功！", list)
}

func GetDeclare(c *gin.Context) {
	declareId := c.Query("id")
	id, _ := strconv.ParseUint(declareId, 10, 64)

	d, err := declare.GetDeclareByID(uint(id))
	if err != nil {
		action.Bad(c, http.StatusInternalServerError, "申报获取失败！")
		return
	}
	action.Ok(c, "申报获取成功！", d)
}

func Create(c *gin.Context) {
	var data model.Declare
	if err := c.Bind(&data); err != nil {
		action.Bad(c, http.StatusBadRequest, "请求参数错误！")
		return
	}

	if err := declare.Insert(data); err != nil {
		action.Bad(c, http.StatusInternalServerError, "添加数据失败！"+err.Error())
		return
	}
	action.Ok(c, "添加成功！", "")
}

func Update(c *gin.Context) {
	var data model.Declare
	if err := c.Bind(&data); err != nil {
		action.Bad(c, http.StatusBadRequest, "请求参数错误！")
		return
	}

	if err := declare.Update(data); err != nil {
		action.Bad(c, http.StatusInternalServerError, "更新数据失败！"+err.Error())
		return
	}
	action.Ok(c, "更新成功！", "")
}
