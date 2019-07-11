-- Sequence: public.file_id

-- DROP SEQUENCE public.file_id;

CREATE SEQUENCE public.file_id
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 74
  CACHE 1;
ALTER TABLE public.file_id
  OWNER TO postgres;
