:: 创建文件夹链接

:: 想要克隆的端文件夹目录 set client_source="F:\TeddyFrameWork"
set client_source=..\YooAsset
:: 克隆后的文件夹目录 set client_destination="F:\TeddyFrameWork_Clone"
set client_destination=..\YooAsset_Clone

:: remove directory 删除文件夹
rmdir %client_destination%
:: make directory 新增文件夹
mkdir %client_destination%

:: 删除旧文件夹
::rmdir %client_destination%\Assets
::rmdir %client_destination%\ProjectSettings
::rmdir %client_destination%\Library
:: 链接 Assets、ProjectSettings、Library 3个文件夹
::mklink /d %client_destination%\Assets %client_source%\Assets
::mklink /d %client_destination%\ProjectSettings %client_source%\ProjectSettings
::mklink /d %client_destination%\Library %client_source%\Library 

mklink /j %client_destination%\Assets Assets
mklink /j %client_destination%\ProjectSettings ProjectSettings
mklink /j %client_destination%\Library Library 

pause


::mklink在我看来，就是创建快捷方式。建立一份引用，指向源地址。

::Mklink的参数定义
::无参数指定：建立文件的符号链接。删除链接文件不会影响源文件
::/d：建立目录的符号链接符号链接(symbolic link)
::/j： 建立目录的软链接（联接）(junction)
::/h：建立文件的硬链接(hard link)
::1.符号链接(symbolic link)
::建立一个软链接相当于建立一个文件（或目录），这个文件（或目录）用于指向别的文件（或目录），和win的快捷方式有些类似。删除这个链接，对原来的文件（或目录）没有影像没有任何影响；而当你删除原文件（或目录）时，再打开链接则会提示“位置不可用”。
::2.软链接（联接）(junction)
::作用基本和符号链接类似。区别在于，软链接在建立时会自动引用原文件（或目录）的绝对路径，而符号链接允许相对路径的引用。
::3.硬链接(hard link)
::建立一个硬链接相当于给文件建立了一个别名，例如对1.TXT创建了名字为2.TXT的硬链接，若使用记事本对1.TXT进行修改，则2.TXT也同时被修改，若删除1.TXT，则2.TXT依然存在，且内容与1.TXT一样。