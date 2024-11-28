package api

import (
	"report-entrance/internal/api/auth"
	"report-entrance/internal/api/declare"
	"report-entrance/internal/middlewares"

	"github.com/gin-gonic/gin"
)

func SetupRouter() *gin.Engine {
	r := gin.Default()

	r.Static("/public", "./public")
	// 后台
	r.StaticFile("/", "./dist/index.html")
	r.Static("/assets", "./dist/assets")
	r.StaticFile("/favicon.ico", "./dist/favicon.ico")
	// h5
	r.StaticFile("/h5", "./h5/index.html")
	r.Static("/static", "./h5/static")
	r.Static("/css", "./h5/css")
	r.Static("/js", "./h5/js")

	public := r.Group("/api")
	{
		// 登录
		public.POST("/login", auth.Login)
		// 申报
		public.GET("/declare/search", declare.SearchByPhone)
		public.POST("/declare", declare.Create)
	}

	protected := r.Group("/api/admin")
	{
		// 在路由组中使用中间件
		protected.Use(middlewares.JwtAuthMiddleware())
		protected.GET("/current/user", auth.CurrentUser)
		protected.GET("/declare/list", declare.GetList)
		protected.GET("/declare/byId", declare.GetDeclare)
		protected.PUT("/declare", declare.Update)
	}
	return r
}
