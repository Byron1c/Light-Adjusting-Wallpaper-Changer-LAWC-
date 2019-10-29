del "E:\TEMP\LAWC 32bit\*.*" /S /Q
del "E:\TEMP\LAWC 64bit\*.*" /S /Q
xcopy E:\Users\Troy\Dropbox\@Backup\VisualStudio\LAWC\LAWC\bin\Debug\*.* /s /e "E:\TEMP\LAWC 32bit\"
xcopy E:\Users\Troy\Dropbox\@Backup\VisualStudio\LAWC\LAWC\bin\x64\Debug\*.* /s /e "E:\TEMP\LAWC 64bit\"
pause