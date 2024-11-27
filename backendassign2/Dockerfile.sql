# Start from the official SQL Server image
FROM mcr.microsoft.com/mssql/server:2022-latest

# Ensure the /var/lib/apt/lists directory exists and has proper permissions
USER root
RUN mkdir -p /var/lib/apt/lists/partial && chmod -R 777 /var/lib/apt/lists

# Install mssql-tools (sqlcmd) and dependencies
RUN apt-get update \
    && apt-get install -y curl apt-transport-https ca-certificates gnupg \
    && curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl https://packages.microsoft.com/config/debian/11/prod.list > /etc/apt/sources.list.d/mssql-release.list \
    && apt-get update \
    && ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev \
    && rm -rf /var/lib/apt/lists/*

# Set the PATH for mssql-tools
ENV PATH="${PATH}:/opt/mssql-tools/bin"
