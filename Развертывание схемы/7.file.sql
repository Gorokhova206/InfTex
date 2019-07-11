-- Table: public.file

-- DROP TABLE public.file;

CREATE TABLE public.file
(
  id integer NOT NULL DEFAULT nextval('file_id'::regclass),
  name character varying(100),
  description character varying(200),
  extension_id integer,
  folder_id integer,
  content text,
  CONSTRAINT "FilePK" PRIMARY KEY (id),
  CONSTRAINT "FileExtensionFK" FOREIGN KEY (extension_id)
      REFERENCES public.extension (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT "FileFolderIdFK" FOREIGN KEY (folder_id)
      REFERENCES public.folder (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE CASCADE
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.file
  OWNER TO postgres;

-- Index: public.file_index

-- DROP INDEX public.file_index;

CREATE INDEX file_index
  ON public.file
  USING btree
  (id);

