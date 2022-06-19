#!/bin/bash
migrationFolder="$(pwd)/GamingProfile-Migrations"
down=${down:-false}
while [ $# -gt 0 ]; do

   if [[ $1 == *"--"* ]]; then
        param="${1/--/}"
        declare $param="$2"
   fi

  shift
done
dotnet run --project "${migrationFolder}" "${down}" "${migrationFolder}"