CREATE TABLE memberships (
   id BIGINT PRIMARY KEY identity(1,1),
   membership_type text NOT NULL,
   start_date date NOT NULL,
   end_date date,
   holder_id INT REFERENCES [user] (id)
);

CREATE TABLE family_members (
   id INT PRIMARY KEY identity(1,1),
   holder_id INT REFERENCES [user] (id),
   first_name text NOT NULL,
   last_name text NOT NULL,
   urlimage text NOT NULL,
   relationship text NOT NULL,
   date_of_birth date NOT NULL,
   gender text NOT NULL default ''
);

CREATE TABLE visits (
   id INT PRIMARY KEY identity(1,1),
   holder_id INT REFERENCES [user] (id),
   family_member_id INT REFERENCES family_members (id),
   visit_date DATETIME DEFAULT GETDATE(),
   reason_for_visit text,
   diagnosis text,
   treatment text
);

CREATE TABLE doctors (
   id INT PRIMARY KEY identity(1,1),
   first_name text NOT NULL,
   last_name text NOT NULL,
   specialty text NOT NULL,
   contact_number text,
   email text
);
ALTER TABLE
   visits
ADD doctor_id INT REFERENCES doctors (id);

CREATE TABLE medical_records (
   id INT PRIMARY KEY identity(1,1),
   holder_id INT REFERENCES [user] (id),
   family_member_id INT REFERENCES family_members (id),
   record_date DATETIME  DEFAULT GETDATE(),
   notes text
);
ALTER TABLE medical_records
ADD allergies text, conditions text, medications text,age INT,[weight] NUMERIC,height NUMERIC, chronic_issues text;