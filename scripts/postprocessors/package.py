import zipfile
import os
import re

def substitute(s,substitutions):
    for key in substitutions:
        s = s.replace(key, substitutions[key])
    return s

def zipAddFileFromTemplate(zip,filename,substitutions):
	dir = 'scripts\\postprocessors\\templates'
	template = open(dir + '\\' + filename + '.template')
	contents = template.read()
	template.close()
	
	contents = substitute(contents,substitutions)
	
	tmpfilename = '$$zaffstemp$$'
	tmp = open(tmpfilename,'w')
	tmp.write(contents)
	tmp.close()
	zip.write(tmpfilename,filename)
	os.remove(tmpfilename)


with zipfile.ZipFile(targetDir+'\\BariVsAddon.vsix', 'w') as myzip:
	myzip.write(targetDir+'\\JetBrains.Annotations.dll','JetBrains.Annotations.dll')
	myzip.write(targetDir+'\\BariVsAddon.dll','BariVsAddon.dll')

	substitutions = {
	'<?= guid >': '99e01380-a443-4a37-af07-527015b79f78',
	'<?= classname >': 'BariVsAddon.BariVsPackagePackage',
	}
	zipAddFileFromTemplate(myzip,'[Content_Types].xml',substitutions)
	zipAddFileFromTemplate(myzip,'extension.vsixmanifest',substitutions)
	zipAddFileFromTemplate(myzip,'BariVsAddon.pkgdef',substitutions)
	

results = ['BariVsAddon.vsix']