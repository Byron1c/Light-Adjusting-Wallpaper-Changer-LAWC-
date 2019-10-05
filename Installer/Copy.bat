@echo off
echo Copy Setup to E:\TEMP\
xcopy "E:\Users\Troy\Dropbox\@Backup\VisualStudio\LAWC Data\Installers\LAWC_Setup_0.9.9.0_BETA.exe" E:\TEMP\ /Y
echo .
echo Copy Setup to Castiel:
rem xcopy "E:\Users\Troy\Dropbox\@Backup\VisualStudio\LAWC Data\Installers\LAWC_Setup_0.9.9.0_BETA.exe" \\castiel\Troy\ /Y
copy "E:\Users\Troy\Dropbox\@Backup\VisualStudio\LAWC Data\Installers\LAWC_Setup_0.9.9.0_BETA.exe" \\castiel\Troy\ /y
echo .
echo Copy Setup to MewTwo
xcopy "E:\Users\Troy\Dropbox\@Backup\VisualStudio\LAWC Data\Installers\LAWC_Setup_0.9.9.0_BETA.exe" \\MEWTWO\Downloads\@Inbox\ /Y
pause