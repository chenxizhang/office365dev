# 详解Office Add-in 清单文件

> 作者：陈希章 发表于2017年9月17日



## 注意事项

1. 确保add-in ID是唯一的，这是一个GUID。如果使用Visual Studio开发的话，可以在工具菜单中，找到Create GUID的一个小工具，但也可以通过其他一些方式生成。
    ![](images/createguid.png)

1. 所有的Url都必须是https的。
1. 所有的图片（例如用在命令按钮上面的图片），都必须是允许缓存，也就是说服务器不能在Header里面添加on-cache/no-store 这样的值。
1. 如果add-in需要发布到Office Store，则必须提供SupportUrl这个属性。
1. 
