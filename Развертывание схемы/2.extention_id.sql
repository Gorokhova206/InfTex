-- Sequence: public.extention_id

-- DROP SEQUENCE public.extention_id;

CREATE SEQUENCE public.extention_id
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 10
  CACHE 1;
ALTER TABLE public.extention_id
  OWNER TO postgres;
