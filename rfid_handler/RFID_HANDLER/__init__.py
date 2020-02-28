# pip install pyserial
# pip install requests
# pip install urllib3
import json
from serial import Serial
import serial.tools.list_ports
import time
import requests as http

default_api_root = "https://localhost:44372/api/cards/"
api_root_choice = input("Default API Root Link: " + default_api_root + "\n" +
                                                                       "Press 'y' to use the default Link \n"
                                                                       "Press 'n' to enter new Link \n")

if api_root_choice == 'n' or api_root_choice == 'N':
    http_protocols = ['http', 'https']
    selected_protocol = 0

    def protocol_selection():
        count = 1
        for http_protocol in http_protocols:
            print(str(count) + ". " + http_protocol)
            count += 1
        try:
            selected_protocol = int(input("Select a protocol: "))
            if selected_protocol > len(http_protocols) and selected_protocol > 0:
                raise Exception
        except:
            print("Wrong Input")
            protocol_selection()
    protocol_selection()
    ip_address_dns = input("Please Enter IP Address or DNS: ")
    port = input("Please Enter Port: ")
    rout = input("Please API Rout.................\n\tEx. '/api/cards/':\n ")
    default_api_root = http_protocols[selected_protocol-1]+"://"+ip_address_dns+":"+port+rout

else:
    print("Using Default API URL.......!")


def is_network_connected():
    try:
        response = http.get(default_api_root, verify=False)
        # print(response.status_code)
        if response.status_code == 200:
            return True
        return False
    except:
        return False


def allocate_card(rfid):
    print(type(rfid))
    try:
        user_id = int(input("Enter User ID: "))
    except:
        print("Wrong Input!")
        allocate_card(rfid)
    response = http.post(default_api_root, verify=False, json={'rfid': rfid, 'userID': user_id})
    if response.status_code == 200:
        print("\n\n\n")
        print(dict(response.json())["errorMessage"])
    elif response.status_code == 201:
        print("New RFID: "+rfid+" Allocated to the user")
    else:
        print("Something went wrong")


def de_allocate_card(rfid):
    response = http.delete(default_api_root + rfid, verify=False)
    if response.status_code == 204:
        print("RFID Details Deleted Successfully")
    else:
        print("Something went wrong!")


def update_card(rfid):
    try:
        user_id = int(input("Enter User ID: "))
    except:
        print("Wrong Input!")
    response = http.put(default_api_root + rfid, verify=False, json={'rfid': rfid, 'userID': user_id})
    if response.status_code == 200:
        print(dict(response.json())["errorMessage"])
    elif response.status_code == 204:
        print("RFID Details updated successfully!")
    else:
        print("Something went wrong!")


def get_details(rfid):
    response = http.get(default_api_root + rfid, verify=False)
    if response.status_code == 200:
        print(json.dumps(response.json(), indent=4))
    else:
        print("Something went wrong!")


def options_selection(rfid):
    o = int(input("Please select an option from the list:\n"
                  "\t 1. Allocate RFID to New User \n"
                  "\t 2. De-Allocate RFID Card \n"
                  "\t 3. Replace RFID Card \n"
                  "\t 4. Get Card Details \n"
                  "\t 5. Exit\n"))
    if o == 1:
        if not is_network_connected():
            print("We are offline Please Check Your internet connection")
            return None
        allocate_card(rfid)
    elif o == 2:
        if not is_network_connected():
            print("We are offline Please Check Your internet connection")
            return None
        de_allocate_card(rfid)
    elif o == 3:
        if not is_network_connected():
            print("We are offline Please Check Your internet connection")
            return None
        update_card(rfid)
    elif o == 4:
        if not is_network_connected():
            print("We are offline Please Check Your internet connection")
            return None
        get_details(rfid)
    else:
        exit()


while True:
    print("Loading available PORTS....")
    ports = list(serial.tools.list_ports.comports(include_links=False))
    index = 1
    print("Please select a port:")
    for port in ports:
        print(index, ".", port.device, port.description)
        index += 1
    try:
        index = int(input("Enter your selection: ")) - 1
        if index > len(ports):
            raise IndexError
    except IndexError:
        print("You have entered wrong option!\n\n\n")
        continue

    except:
        print("Please Enter the right input!\n\n\n")
        continue

    working_port = str(ports[index].device).lower()
    baud_rate = 9600
    sr = Serial(working_port, baud_rate)
    print("Please wait! while connecting to device.")
    time.sleep(5)
    print("\n\n\n\n\nPlease tap the card on scanner: \n\n\n")
    while True:
        if sr.inWaiting() > 0:
            data = sr.readline()
            sr.flush()
            sr.flushInput()
            if len(data.decode()) >= 10:
                options_selection(str(data.decode()).strip(" ").strip("\r\n"))
                sr.flushInput()
            print("\n\n\n\nPlease tap the card on scanner: \n\n\n")
            # print(data.decode())
