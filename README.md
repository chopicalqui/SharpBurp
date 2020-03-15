# SharpBurp

C# application, which parses a Nmap XML output file and allows sending 
selected HTTP services to the BurpSuite scanner.

## Configuration

In order to use BurpSharp, BurpSuite's REST API must be activated as 
depicted in the following screenshot:
![BurpSuite API configuration for BurpSharp](Resources/burpsuite_config.png)

During BurpSuite's setup, copy the configuration to SharpBurp as depicted 
in the following screenshot. Note that SharpBurp will permanently 
store all configuration:
![SharpBurp configuration to interact with BurpSuite](Resources/burpsuite_config.png)

## Usage

  1. Load one or more Nmap XML scan result files into SharpBurp by using 
  the button 'Load Nmap XML'.
  2. Tell SharpBurp which services shall be scanned by checking or 
  unchecking the respective checkboxes in column 'Scan'. Note that 
  the table implements a context menue that allows you to check or 
  uncheck multiple selected rows.
  3. Click button 'Send to Burp API' to send the selected services to 
  the BurpSuite Scanner.
  
## Author

  * **Lukas Reiter** (@chopicalquy) - *Initial Work* - [SharpBurp](https://github.com/chopicalqui/SharpBurp)

## License

This project is licensed under the GPLv3 License - see the
[license](https://github.com/chopicalqui/SharpBurp/blob/master/LICENSE) file for details.