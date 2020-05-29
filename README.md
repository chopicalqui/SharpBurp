# SharpBurp

C# application, which parses Nmap or Nessus XML output files and allows sending 
selected HTTP services to the BurpSuite Scanner via BurpSuite's REST API.
Use this application to start large-scale web application security scans
based on Nmap or Nessus scan results.

## Configuration

In order to use BurpSharp, BurpSuite's REST API must be activated as 
depicted in the following screenshot:
![BurpSuite API configuration for BurpSharp](Resources/burpsuite_config.png)

During BurpSuite's setup, copy the configuration to SharpBurp as depicted 
in the following screenshot. Note that SharpBurp will permanently 
store all configuration:
![SharpBurp configuration to interact with BurpSuite](Resources/sharpburp_config.png)

## Usage

  1. Load one or more Nmap/Nessus XML scan result files into SharpBurp by using 
  the button 'Load Nmap XML' or 'Load Nessus'. In case of Nmap, SharpBurp, per default, 
  only imports open ports (see checkbox 'Import Open') (Nessus reports only open ports, 
  therefore the status filters are irrelevant).
  2. Tell SharpBurp, which URLs shall be sent to the BurpSuite Scanner 
  by checking or unchecking the respective checkboxes in column 'Scan'. In
  addition, the table's context menu can be used to check or uncheck multiple 
  rows. Thereby, only HTTP services can be scanned (rows that contain 'http' in 
  column 'Nmap Name New').
  2. Choose BurpSuite's scan configuration and resource pool that shall be 
  used for the scans. The 'Scan Size' field tells SharpBurp how many URLs shall
  be scanned by one single BurpSuite scan. In other words, the total number of
  BurpSuite scans is the total number of selected rows (see status bar) 
  divided by the 'Scan Size'.
  5. Click button 'Send to Burp API' to send the selected URLs to the BurpSuite 
  Scanner. Note that SharpBurp keeps track which URLs have already been sent to 
  BurpSuite and therefore, only sends each URL once.
  
## Author

  * **Lukas Reiter** (@chopicalquy) - [SharpBurp](https://github.com/chopicalqui/SharpBurp)

## License

This project is licensed under the GPLv3 License - see the
[license](https://github.com/chopicalqui/SharpBurp/blob/master/LICENSE) file for details.
