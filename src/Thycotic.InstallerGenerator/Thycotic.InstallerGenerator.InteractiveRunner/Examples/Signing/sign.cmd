echo use password as password1

makecert -r -pe -n "CN=Secret Server Distributed Engine Temporary CA" -ss CA -sr CurrentUser ^
		-a sha256 -cy authority -sky signature -sv SSDECA.pvk SSDECA.cer 
		
REM import to cert store
REM certutil -user -addstore Root SSDECA.cer

makecert -pe -n "CN=Secret Server Distributed Engine SPC" -a sha256 -cy end ^
         -sky signature ^
         -ic SSDECA.cer -iv SSDECA.pvk ^
         -sv SSDESPC.pvk SSDESPC.cer
	
pvk2pfx -pvk SSDESPC.pvk -spc SSDESPC.cer -pfx SSDESPC.pfx -po password1

signtool sign /v /f SSDESPC.pfx Thycotic.DistributedEngine.Service.5.0.msi
