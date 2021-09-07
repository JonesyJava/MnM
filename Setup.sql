CREATE TABLE profiles (
  id VARCHAR(255) NOT NULL,
  email VARCHAR(255) NOT NULL,
  name VARCHAR(255),
  picture VARCHAR(255),
  PRIMARY KEY (id)
);
CREATE TABLE doctors (
  id INT NOT NULL AUTO_INCREMENT,
  name VARCHAR(255) NOT NULL,
  specialty VARCHAR(255),
  creatorId VARCHAR(255) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (creatorId) REFERENCES profiles (id) ON DELETE CASCADE
);
CREATE TABLE patients (
  id INT NOT NULL AUTO_INCREMENT,
  name VARCHAR(255) NOT NULL,
  birthDate INT NOT NULL,
  creatorId VARCHAR(255) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (creatorId) REFERENCES profiles (id) ON DELETE CASCADE
);
CREATE TABLE wishlistproducts (
  id INT NOT NULL AUTO_INCREMENT,
  doctorId INT,
  patientId INT,
  creatorId VARCHAR(255),
  PRIMARY KEY (id),
  FOREIGN KEY (creatorId) REFERENCES profiles (id) ON DELETE CASCADE,
  FOREIGN KEY (doctorId) REFERENCES doctors (id) ON DELETE CASCADE,
  FOREIGN KEY (patientId) REFERENCES patients (id) ON DELETE CASCADE
)