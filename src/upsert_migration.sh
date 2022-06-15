#!/bin/bash

echo "Enter Story Number"
read storyNumber
echo "Enter Migration Name:" 
read migrationName
echo "You want to add script file ? (y/n)"
read isAddMigrationFile
timestamp=$(date "+%Y%m%d%H%M%S")
migrationFileName="${timestamp}_${storyNumber}_${migrationName}"
migrationStatement="Execute.Sql(\"\")"
scriptFilePath="$(pwd)/GamingProfile-Migrations/Migrations/Scripts"
if [ $isAddMigrationFile == "y" ]
then
    upFileContent="--Up migration for ${migrationFileName}"
    upMigrationStatement="Execute.Script(@\"Scripts/${migrationFileName}_Up.sql\");"
    echo "${upFileContent}" >> "${scriptFilePath}/${migrationFileName}_Up.sql"

    downFileContent="--Down migration for ${migrationFileName}"
    downMigrationStatement="Execute.Script(@\"Scripts/${migrationFileName}_Down.sql\");"
    echo "${downFileContent}" >> "${scriptFilePath}/${migrationFileName}_Down.sql"
fi
migrationClassPath="$(pwd)/GamingProfile-Migrations/Migrations"
migrationClassName="GP${storyNumber}_${migrationName}"
migrationClassContent="using FluentMigrator;

namespace GamingProfileMigrations.Migrations
{
    [Migration(${timestamp})]
    public class ${migrationClassName} : Migration
    {
        public override void Up()
        {          
            ${upMigrationStatement}
        }

        public override void Down()
        {
            ${downMigrationStatement}
        }
    }
}"
echo "${migrationClassContent}" >> "${migrationClassPath}/${migrationFileName}.Migration.cs"
echo "Migration added sucessfully"
read x