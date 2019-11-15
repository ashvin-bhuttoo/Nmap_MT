# Nmap_MT
Nmap_MT is an Nmap Wrapper Gui that allows for faster Nmap scans by distributing hosts on multiple parallel threads.
The user has complete control on how many threads may be used in scans and how many addresses should be scanned serially per thread. 

The --min-parallelism <numprobes>; --max-parallelism <numprobes> (Adjust probe parallelization) nmap options may be used in combination with this tool for very large subnet scans.

This project depends on the NMAP project, and Nmap for windows should be installed before this tool can be used.
https://nmap.org/download.html

The latest installer can be downloaded from here: <a href="https://raw.githubusercontent.com/ashvin-bhuttoo/Nmap_MT/master/NmapMT_setup/Release/NmapMT_setup.msi"> v1.0.0</a> 

![alt text](https://i.postimg.cc/JhpVmsby/2019-11-13-16-04-44-Nmap-MT-v1-0-0-0.png)

If you want to buy me a beer, here's the button for it.. :)<br/>
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://paypal.me/ABhuttoo?locale.x=en_US)

This project was built using VS2019 Community Edition.
