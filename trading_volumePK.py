import requests
import json
import csv
import time,datetime,os
from bs4 import BeautifulSoup


dt = datetime.datetime.now()
dt.year
dt.month

stock_No = input("股票代號:")
startYear_range = int(input("開始年份:"))
now =datetime.datetime.now()
year_list = range(startYear_range,now.year+1)
month_list =range(1,13)

def get_webmsg(year,month,stock_id):
    date = str(year) + "{0:0=2d}".format(month) + '01'
    sid = str(stock_id)
    url_twse = 'http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date='+date+'&stockNo='+sid
    res =requests.post(url_twse,)
    soup = BeautifulSoup(res.text , 'html.parser')
    smt = json.loads(soup.text)     #convert data into json
    return smt


def write_csv(stock_id,directory,filename,smt):
    writefile = directory + filename
    outputFile = open(writefile,'w',newline='')
    outputWriter = csv.writer(outputFile)
    head = ''.join(smt['title'].split())
    a = [head,""]
    outputWriter.writerow(a)
    outputWriter.writerow(smt['fields'])
    for data in (smt['data']):
        outputWriter.writerow(data)

    outputFile.close()


def makedirs(year,month,stock_id):
    sid = str(stock_No)
    yy = str(year)
    mm = str(month)
    directory = 'D:/stock'+'/'+sid +'/'+ yy
    if not os.path.isdir(directory):
        os.makedirs (directory)


for year in year_list:
    for month in month_list:
        if(dt.year == year and month > dt.month):break
        sid = str(stock_No)
        yy = str(year)
        mm = month
        directory = 'D:/stock/'+sid+'/'+yy+'/'
        filename = str(yy)+str("%02d"%mm)+'.csv'
        smt = get_webmsg(year,month,stock_No)
        makedirs(year,month,stock_No)
        write_csv(stock_No,directory,filename,smt)
        time.sleep(18)







