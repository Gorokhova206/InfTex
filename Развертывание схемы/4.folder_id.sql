-- Sequence: public.folder_id

-- DROP SEQUENCE public.folder_id;

CREATE SEQUENCE public.folder_id
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 1
  CACHE 1;
ALTER TABLE public.folder_id
  OWNER TO postgres;
