import os, blowfish, base64, json, math

result_path = '../Project/Assets/Resources/Metas'

# Result File Dir
try:
    os.makedirs(result_path)
except OSError as exc:
    pass

file_list = os.listdir("./Datas/")
file_list = sorted(file_list)
file_list_csv = [file for file in file_list if file.endswith(".csv")]

cipher = blowfish.Cipher(b"MafGamesMergeVillage")

import csv
import json
import glob
import os

ignore_list = ['Localization.txt']
for name in file_list_csv:
    data_encrypted = None
    with open("./Datas/" + name, 'rt', encoding='UTF8') as file:
        dataStr = list(csv.DictReader(file))

        if name in ignore_list:
            data_encrypted = json.dumps(dataStr)
        else:
            dataStr = json.dumps(dataStr)
            blocks = math.ceil(len(dataStr) / 8)

            encryptedString = ""
            for i in range(0, blocks):
                block = dataStr[i * 8 : i * 8 + 8]
                if len(block) < 8:
                    count = 8 - len(block)
                    while 0 < count:
                        block += "\0"
                        count = count - 1

                encryptedString += block

            print(len(encryptedString))

            data_encrypted = b"".join(cipher.encrypt_ecb(encryptedString.encode()))
            data_encrypted = base64.b64encode(data_encrypted)

    if data_encrypted != None:
        with open(result_path + '/' + name.replace("csv", "txt"), "wt", encoding='UTF8') as file:
            if name in ignore_list:
                file.write(data_encrypted)
            else:
                file.write(data_encrypted.decode('UTF-8'))

    print('Complete Encrypted - FileName : ' + name)

