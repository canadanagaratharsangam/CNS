CREATE TABLE Address (
    address_id     INTEGER PRIMARY KEY AUTOINCREMENT,
    address_line_1 TEXT,
    address_line_2 TEXT,
    city           TEXT    NOT NULL,
    province       TEXT    NOT NULL,
    postal_code    TEXT
);

CREATE TABLE Contacts (
    contact_id    INTEGER PRIMARY KEY AUTOINCREMENT,
    first_name    TEXT    NOT NULL,
    last_name     TEXT,
    middle_name   TEXT,
    email_address TEXT    UNIQUE,
    address_id    INTEGER REFERENCES Address (address_id) 
);

CREATE TABLE PhoneType (
    phone_type_id INTEGER PRIMARY KEY AUTOINCREMENT,
    phone_type    TEXT    NOT NULL
                          UNIQUE
);

CREATE TABLE Phone (
    phone_id      INTEGER PRIMARY KEY AUTOINCREMENT,
    contact_id    INTEGER REFERENCES Contacts (contact_id),
    phone_type_id INTEGER REFERENCES PhoneType (phone_type_id),
    phone_number  TEXT    NOT NULL
);

CREATE TABLE RelationshipType (
    relationshiptype_id INTEGER PRIMARY KEY AUTOINCREMENT,
    relationshiptype    TEXT    UNIQUE
                                NOT NULL
);

CREATE TABLE Relationship (
    relationship_id       INTEGER PRIMARY KEY AUTOINCREMENT,
    contact_id            INTEGER REFERENCES Contacts (contact_id) 
                                  NOT NULL
                                  UNIQUE,
    related_to_contact_id INTEGER REFERENCES Contacts (contact_id),
    relationshiptype_id   INTEGER REFERENCES RelationshipType (relationshiptype_id) 
);
