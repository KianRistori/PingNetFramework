# PingNetFramework

This is a simple **C#** program that recreate the **"Ping"** command on a hostname or IP address specified by the user. Specifically:
1. The program accepts user input and divides it into a series of commands and function calls using the `Command()` and `CommandReserch()` methods.
2. It uses the `Ping` class to send Internet Control Message Protocol **(ICMP)** packets to the specified hostname or IP address.
3. If the host responds, the program prints the host's address, the packet travel time, and the **TTL** (Time to Live) value of the packet's option.
4. In addition, the program calculates and prints some statistics on the host's response, such as the number of packets sent, received, and lost, and the minimum, maximum, and average packet travel time.
