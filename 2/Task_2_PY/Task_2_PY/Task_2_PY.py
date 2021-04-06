import os
import hashlib
for file in os.listdir(os.getcwd()):
	f=open(file, 'r')
	hex = hashlib.sha3_256(f.read().encode('utf-8')).hexdigest()
	print(file+'\t'+str(hex))
	f.close()