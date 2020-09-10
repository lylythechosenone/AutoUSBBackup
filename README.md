# AutoUSBBackup

This is a Windows application, written in C#, that automatically copies files to a USBflash drive, when plugged in. To set it up, edit config.json. These are the fields that must be filled:<br><br>
<b>driveLetter:</b> `<string>` The letter assigned to your USB drive<br>
<b>pathToCopy:</b> `<string>` The path to be copied to your USB drive<br>
<b>clear:</b> `<bool>` Clear the USB drive before copying
