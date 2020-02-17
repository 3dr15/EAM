from serial import Serial
import time
import requests as http

sr = Serial("com7", 9600)
time.sleep(5)


def alocateCard(rfid):
    response = http.post("https://localhost:44372/api/cards", verify=False, data={'rFID': rfid, 'userID': 1})
    if response.status_code == 201:
        print("TRUE")
    else:
        print("FALSE")


while True:
    if sr.inWaiting() > 0:
        data = sr.readline()
        if len(data.decode()) >= 10:
            alocateCard(data.decode())
        print(data.decode())

