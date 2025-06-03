FROM mcr.microsoft.com/mssql-tools
copy . .

RUN sed -i 's/\r$//' /init.sh
RUN chmod +x /init.sh

CMD ["./init.sh"]