### Cananka ###

A Raspberry Pi HAT for the CAN bus.


#### Bringing the interface up ####

    sudo ip link set can0 up type can bitrate 125000
    ip -d link show can0

For this to happen automatically, just execute:

    sudo bash -c 'echo >> /etc/network/interfaces'
    sudo bash -c 'echo "auto can0" >> /etc/network/interfaces'
    sudo bash -c 'echo "iface can0 inet manual" >> /etc/network/interfaces'
    sudo bash -c 'echo "    pre-up ip link set \$IFACE type can bitrate 125000" >> /etc/network/interfaces'
    sudo bash -c 'echo "    up /sbin/ifconfig \$IFACE up" >> /etc/network/interfaces'
    sudo bash -c 'echo "    down /sbin/ifconfig \$IFACE down" >> /etc/network/interfaces'


#### Testing ####
    
For testing install can-utils:

    sudo apt-get install can-utils

Then you can use cansend and candump (in two different windows), e.g.:

    cansend can0 02A#FEED
    candump can0

---

*You can check my blog and other projects at [www.medo64.com](https://www.medo64.com/2016/05/cananka-the-raspberry-pi-hat/).*
