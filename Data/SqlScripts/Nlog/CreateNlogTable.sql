CREATE TABLE IF NOT EXISTS public.NLog
(
    ID          SERIAL PRIMARY KEY,
    MachineName VARCHAR(200),
    Logged      TEXT        NOT NULL,
    Level       VARCHAR(10) NOT NULL,
    Message     TEXT        NOT NULL,
    Logger      VARCHAR(300),
    Callsite    VARCHAR(300),
    Exception   TEXT
);