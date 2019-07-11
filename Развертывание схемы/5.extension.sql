-- Table: public.extension

-- DROP TABLE public.extension;

CREATE TABLE public.extension
(
  id integer NOT NULL DEFAULT nextval('extention_id'::regclass),
  type character varying(50),
  image character varying,
  CONSTRAINT "ExtensionPK" PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.extension
  OWNER TO postgres;

-- Index: public.extension_index

-- DROP INDEX public.extension_index;

CREATE INDEX extension_index
  ON public.extension
  USING btree
  (id, type COLLATE pg_catalog."default");

